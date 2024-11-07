<template>
    <div class="notification-container">
      <!-- Header -->
      <div class="header">
        <h2>Thông báo</h2>
      </div>
  
      <!-- Notification List -->
      <div class="notification-list">
        <div class="notification-item" v-for="(notification, index) in notifications" :key="index">
          <div class="icon-container">
            <i :class="notification.iconClass"></i>
          </div>
          <div class="content">
            <p class="message">{{ notification.message }}</p>
            <span class="timestamp">{{ formatDate(notification.timestamp) }}</span>
          </div>
        </div>
      </div>
    </div>
  </template>
  
  <script setup>
  import { ref } from 'vue';
  
  // Example notification data
  const notifications = ref([
    {
      iconClass: 'fas fa-comment',
      message: 'Bạn có một bình luận mới trên bài viết của mình.',
      timestamp: new Date(),
    },
    {
      iconClass: 'fas fa-user-plus',
      message: 'Người dùng mới đã theo dõi bạn.',
      timestamp: new Date('2024-11-01T10:30:00'),
    },
    {
      iconClass: 'fas fa-bell',
      message: 'Hệ thống sẽ bảo trì vào lúc 22:00 hôm nay.',
      timestamp: new Date('2024-11-05T14:00:00'),
    },
  ]);
  
  const formatDate = (date) => {
    return new Date(date).toLocaleDateString('vi-VN', {
      year: 'numeric',
      month: 'long',
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit',
    });
  };
  </script>
  
  <style scoped>
  .notification-container {
    background-color: #ffffff;
    border-radius: 10px;
    box-shadow: 0 4px 15px rgba(0, 0, 0, 0.1);
    max-width: 600px;
    margin: 20px auto;
    font-family: "Segoe UI", Tahoma, Geneva, Verdana, sans-serif;
    padding: 20px;
  }
  
  .header {
    border-bottom: 1px solid #e0e0e0;
    padding-bottom: 10px;
    margin-bottom: 15px;
    text-align: center;
  }
  
  .header h2 {
    font-size: 24px;
    color: #0073b1;
    font-weight: 700;
  }
  
  .notification-list {
    display: flex;
    flex-direction: column;
    gap: 15px;
  }
  
  .notification-item {
    display: flex;
    align-items: center;
    padding: 10px;
    background-color: #f8f9fa;
    border-radius: 8px;
    transition: background-color 0.3s;
  }
  
  .notification-item:hover {
    background-color: #e9ecef;
  }
  
  .icon-container {
    font-size: 24px;
    color: #0073b1;
    margin-right: 15px;
  }
  
  .content {
    display: flex;
    flex-direction: column;
  }
  
  .message {
    font-size: 16px;
    color: #333;
    margin: 0;
  }
  
  .timestamp {
    font-size: 12px;
    color: #888;
    margin-top: 5px;
  }
  </style>
  