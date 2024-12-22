// chat.js: Client-side SignalR and Chat Interaction Logic

// Main chat application module
const ChatApp = {
    // SignalR connection
    connection: null,

    // Current user information
    currentUser: {
        id: null,
        username: null
    },

    // Initialize the chat application
    init: function () {
        this.cacheDOM();
        this.bindEvents();
        this.initializeSignalRConnection();
    },

    // Cache DOM elements
    cacheDOM: function () {
        this.contactsList = document.querySelector('.contact-list');
        this.chatMessages = document.querySelector('.chat-messages');
        this.messageInput = document.querySelector('.chat-input input');
        this.sendButton = document.querySelector('.chat-input button.btn-primary');
        this.searchInput = document.querySelector('.search-bar input');
    },

    // Bind event listeners
    bindEvents: function () {
        // Send message on button click
        this.sendButton.addEventListener('click', () => this.sendMessage());

        // Send message on Enter key press
        this.messageInput.addEventListener('keypress', (e) => {
            if (e.key === 'Enter') {
                this.sendMessage();
            }
        });

        // Contact search functionality
        this.searchInput.addEventListener('input', this.filterContacts.bind(this));
    },

    // Initialize SignalR connection
    initializeSignalRConnection: function () {
        // Create connection to the SignalR hub
        this.connection = new signalR.HubConnectionBuilder()
            .withUrl("/chatHub")
            .withAutomaticReconnect()
            .build();

        // Handle incoming messages
        this.connection.on("ReceiveMessage", this.renderMessage.bind(this));

        // Handle private messages
        this.connection.on("ReceivePrivateMessage", this.renderPrivateMessage.bind(this));

        // Start the connection
        this.connection.start()
            .then(() => {
                console.log("SignalR Connected");
                this.setUserStatus('online');
            })
            .catch(err => console.error("SignalR Connection Error:", err));
    },

    // Send a message
    sendMessage: function () {
        const message = this.messageInput.value.trim();
        if (!message) return;

        // Determine if it's a group or private message
        const selectedContact = this.getSelectedContact();

        if (selectedContact) {
            // Private message
            this.connection.invoke("SendPrivateMessage",
                this.currentUser.username,
                selectedContact.username,
                message
            );
        } else {
            // Group message
            this.connection.invoke("SendMessage",
                this.currentUser.username,
                message
            );
        }

        // Clear input and add message to chat
        this.messageInput.value = '';
        this.renderOwnMessage(message);
    },

    // Render an incoming message
    renderMessage: function (username, message, timestamp) {
        const messageElement = this.createMessageElement(username, message, timestamp, false);
        this.chatMessages.appendChild(messageElement);
        this.scrollToBottom();
    },

    // Render a private message
    renderPrivateMessage: function (fromUser, message, timestamp) {
        const messageElement = this.createMessageElement(fromUser, message, timestamp, false);
        this.chatMessages.appendChild(messageElement);
        this.scrollToBottom();
    },

    // Render user's own message
    renderOwnMessage: function (message) {
        const messageElement = this.createMessageElement(
            this.currentUser.username,
            message,
            new Date(),
            true
        );
        this.chatMessages.appendChild(messageElement);
        this.scrollToBottom();
    },

    // Create message element
    createMessageElement: function (username, message, timestamp, isOwn = false) {
        const messageDiv = document.createElement('div');
        messageDiv.classList.add('message');
        messageDiv.classList.add(isOwn ? 'sent-message' : 'received-message');

        messageDiv.innerHTML = `
            <strong>${isOwn ? 'You' : username}</strong>
            <p>${message}</p>
            <small class="text-muted">${this.formatTimestamp(timestamp)}</small>
        `;

        return messageDiv;
    },

    // Format timestamp
    formatTimestamp: function (timestamp) {
        const date = new Date(timestamp);
        return date.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
    },

    // Scroll chat to bottom
    scrollToBottom: function () {
        this.chatMessages.scrollTop = this.chatMessages.scrollHeight;
    },

    // Filter contacts based on search
    filterContacts: function (e) {
        const searchTerm = e.target.value.toLowerCase();
        const contactItems = this.contactsList.querySelectorAll('.contact-item');

        contactItems.forEach(contact => {
            const username = contact.querySelector('.contact-name').textContent.toLowerCase();
            contact.style.display = username.includes(searchTerm) ? 'flex' : 'none';
        });
    },

    // Load contacts
    loadContacts: function () {
        // Fetch contacts from server
        fetch('/Chat/GetContacts')
            .then(response => response.json())
            .then(contacts => {
                this.contactsList.innerHTML = ''; // Clear existing contacts
                contacts.forEach(contact => {
                    const contactElement = this.createContactElement(contact);
                    this.contactsList.appendChild(contactElement);
                });
            })
            .catch(error => console.error('Error loading contacts:', error));
    },

    // Create contact list item
    createContactElement: function (contact) {
        const contactDiv = document.createElement('div');
        contactDiv.classList.add('contact-item', 'p-2', 'd-flex', 'align-items-center');
        contactDiv.innerHTML = `
            <img src="${contact.profilePicture}" class="rounded-circle me-3" width="50" height="50">
            <div>
                <h6 class="contact-name m-0">${contact.username}</h6>
                <small class="text-muted">${contact.isOnline ? 'Online' : 'Offline'}</small>
            </div>
        `;

        // Add click event to open chat with this contact
        contactDiv.addEventListener('click', () => this.openChat(contact));

        return contactDiv;
    },

    // Open chat with a specific contact
    openChat: function (contact) {
        // Update chat header
        const chatHeader = document.querySelector('.chat-header');
        chatHeader.querySelector('img').src = contact.profilePicture;
        chatHeader.querySelector('h6').textContent = contact.username;

        // Load chat history
        this.loadChatHistory(contact);
    },

    // Load chat history with a contact
    loadChatHistory: function (contact) {
        fetch(`/Chat/GetChatHistory?contactId=${contact.id}`)
            .then(response => response.json())
            .then(messages => {
                // Clear existing messages
                this.chatMessages.innerHTML = '';

                // Render previous messages
                messages.forEach(msg => {
                    const messageElement = this.createMessageElement(
                        msg.senderId === this.currentUser.id ? 'You' : contact.username,
                        msg.content,
                        msg.timestamp,
                        msg.senderId === this.currentUser.id
                    );
                    this.chatMessages.appendChild(messageElement);
                });

                this.scrollToBottom();
            })
            .catch(error => console.error('Error loading chat history:', error));
    },

    // Set user online/offline status
    setUserStatus: function (status) {
        // Invoke method on SignalR hub to update status
        this.connection.invoke("SetUserStatus", this.currentUser.id, status)
            .catch(err => console.error("Failed to set user status:", err));
    },

    // Handle user going offline (page close/unload)
    setupOfflineHandling: function () {
        window.addEventListener('beforeunload', () => {
            this.setUserStatus('offline');
        });
    }
};

// Initialize the chat app when DOM is fully loaded
document.addEventListener('DOMContentLoaded', () => {
    ChatApp.init();
});

// Error handling for SignalR
function handleSignalRError(error) {
    console.error('SignalR Error:', error);
    // Optionally show a user-friendly error notification
    alert('Connection error. Please check your internet connection.');
}