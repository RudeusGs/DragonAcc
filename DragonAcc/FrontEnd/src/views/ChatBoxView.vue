<template>
    <div class="messaging-container">
      <!-- Sidebar -->
      <div class="sidebar">
        <div class="header">
          <h2>Nhắn tin</h2>
          <i class="fas fa-comments header-icon"></i>
        </div>
        <div class="contacts">
          <div class="contact" v-for="contact in contacts" :key="contact.id">
            <img :src="contact.avatar" alt="Avatar" class="contact-avatar" />
            <div class="contact-info">
              <span class="contact-name">{{ contact.name }}</span>
              <span class="last-message">{{ contact.lastMessage }}</span>
            </div>
          </div>
        </div>
      </div>
      
      <!-- Chat Area -->
      <div class="chat-area">
        <div class="chat-header">
          <img :src="selectedContact.avatar" alt="Avatar" class="chat-avatar" />
          <h3 class="chat-name">{{ selectedContact.name }}</h3>
          <i class="fas fa-ellipsis-v chat-options"></i>
        </div>
        <div class="chat-messages">
          <div class="message" v-for="message in messages" :key="message.id" :class="{ 'my-message': message.sender === 'me' }">
            <div class="message-bubble">{{ message.text }}</div>
            <span class="message-time">{{ message.time }}</span>
          </div>
        </div>
        <div class="chat-input-area">
          <input type="text" placeholder="Nhập tin nhắn..." class="chat-input" v-model="newMessage" />
          <i class="fas fa-paper-plane send-icon" @click="sendMessage"></i>
        </div>
      </div>
    </div>
  </template>
  
  <style scoped>
  /* Main Container */
  .messaging-container {
    display: flex;
    height: 100vh;
    background-color: #f3f6fb;
    font-family: "Segoe UI", Tahoma, Geneva, Verdana, sans-serif;
    overflow: hidden;
  }
  
  /* Sidebar Styling */
  .sidebar {
    width: 300px;
    background-color: #ffffff;
    box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
    padding: 20px;
    overflow-y: auto;
    transition: transform 0.3s ease;
  }
  
  .header {
    display: flex;
    align-items: center;
    justify-content: space-between;
    margin-bottom: 20px;
  }
  
  .header h2 {
    font-size: 24px;
    color: #333;
    margin: 0;
  }
  
  .header-icon {
    font-size: 24px;
    color: #0073b1;
  }
  
  .contacts {
    display: flex;
    flex-direction: column;
    gap: 15px;
  }
  
  .contact {
    display: flex;
    align-items: center;
    gap: 10px;
    padding: 10px;
    border-radius: 8px;
    cursor: pointer;
    transition: background-color 0.3s;
  }
  
  .contact:hover {
    background-color: #f0f0f0;
  }
  
  .contact-avatar {
    width: 40px;
    height: 40px;
    border-radius: 50%;
    object-fit: cover;
  }
  
  .contact-info {
    display: flex;
    flex-direction: column;
  }
  
  .contact-name {
    font-size: 16px;
    font-weight: bold;
    color: #333;
  }
  
  .last-message {
    font-size: 14px;
    color: #888;
  }
  
  /* Chat Area Styling */
  .chat-area {
    flex: 1;
    display: flex;
    flex-direction: column;
    background-color: #ffffff;
    border-left: 1px solid #ddd;
    padding: 20px;
    overflow: hidden;
  }
  
  .chat-header {
    display: flex;
    align-items: center;
    justify-content: space-between;
    border-bottom: 1px solid #ddd;
    padding-bottom: 10px;
    margin-bottom: 20px;
  }
  
  .chat-avatar {
    width: 40px;
    height: 40px;
    border-radius: 50%;
    object-fit: cover;
  }
  
  .chat-name {
    font-size: 18px;
    color: #333;
    margin: 0;
  }
  
  .chat-options {
    font-size: 20px;
    color: #888;
    cursor: pointer;
  }
  
  .chat-messages {
    flex: 1;
    overflow-y: auto;
    padding-right: 10px;
    display: flex;
    flex-direction: column;
    gap: 10px;
  }
  
  .message {
    display: flex;
    flex-direction: column;
    max-width: 70%;
    margin-bottom: 10px;
  }
  
  .my-message {
    align-self: flex-end;
    text-align: right;
  }
  
  .message-bubble {
    background-color: #e1f5fe;
    color: #333;
    padding: 10px;
    border-radius: 8px;
    font-size: 14px;
    transition: background-color 0.3s;
  }
  
  .my-message .message-bubble {
    background-color: #0073b1;
    color: white;
  }
  
  .message-time {
    font-size: 12px;
    color: #aaa;
    margin-top: 5px;
  }
  
  .chat-input-area {
    display: flex;
    align-items: center;
    border-top: 1px solid #ddd;
    padding-top: 10px;
    margin-top: 10px;
  }
  
  .chat-input {
    flex: 1;
    border: none;
    padding: 10px;
    font-size: 16px;
    border-radius: 4px;
    outline: none;
    background-color: #f0f0f0;
    margin-right: 10px;
  }
  
  .send-icon {
    font-size: 24px;
    color: #0073b1;
    cursor: pointer;
    transition: color 0.3s;
  }
  
  .send-icon:hover {
    color: #005582;
  }
  
  /* Smooth transitions for animations */
  .sidebar,
  .contact,
  .message-bubble,
  .send-icon {
    transition: all 0.3s ease;
  }
  
  </style>
  
  <script setup lang="ts">
import { ref } from 'vue';

  // Example data for contacts and messages
  const contacts = [
    { id: 1, name: 'John Doe', avatar: 'https://via.placeholder.com/40', lastMessage: 'Hey, how are you?' },
    { id: 2, name: 'Jane Smith', avatar: 'https://via.placeholder.com/40', lastMessage: 'See you tomorrow!' },
    { id: 3, name: 'Alice Brown', avatar: 'https://via.placeholder.com/40', lastMessage: 'Thank you!' }
  ];
  
  const selectedContact = {
    id: 1,
    name: 'John Doe',
    avatar: 'https://via.placeholder.com/40'
  };
  
  const messages = [
    { id: 1, text: 'Hello!', time: '10:00 AM', sender: 'me' },
    { id: 2, text: 'How are you?', time: '10:05 AM', sender: 'John' },
    { id: 3, text: 'I am good, thanks!', time: '10:10 AM', sender: 'me' }
  ];
  
  const newMessage = ref('');
  
  const sendMessage = () => {
    if (newMessage.value.trim()) {
      messages.push({
        id: messages.length + 1,
        text: newMessage.value,
        time: new Date().toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' }),
        sender: 'me'
      });
      newMessage.value = '';
    }
  };
  </script>
  