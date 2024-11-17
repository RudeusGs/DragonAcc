<template>
  <div class="notification-container">
    <h2 class="notification-header">Thông báo</h2>
    <div v-if="notifications.length" class="notifications-list">
      <div 
        v-for="notification in notifications" 
        :key="notification.id" 
        :class="[
          'notification', 
          { 
            'notification-read': notification.isRead,
            'notification-unread': !notification.isRead
          }
        ]" 
        @click="markAsRead(notification)"
      >
        <img :src="notificationIcon" alt="Notification Icon" class="notification-icon" />
        <div class="notification-text">
          <p :class="{ 'unread-text': !notification.isRead }">{{ notification.content }}</p>
          <span class="time">{{ notification.timeAgo || 'Vừa xong' }}</span>
        </div>
      </div>
    </div>
    <div v-else class="no-notifications">Không có thông báo</div>
  </div>
</template>

<script setup lang="ts">
import { onMounted, ref, reactive } from 'vue';
import notificationApi from '@/api/notification.api';
import { userStore } from '@/stores/auth';
import notificationGif from '@/assets/notification.gif';

// Định nghĩa cấu trúc của đối tượng Notification
interface Notification {
  id: number;
  content: string;
  isRead: boolean;
  createdDate: string;
  timeAgo?: string;
}

// Tham chiếu phản ứng
const notifications = ref<Notification[]>([]);
const notificationIcon = ref(notificationGif);

// Lấy user ID từ store hoặc localStorage
const store = userStore();
const userId = store.user?.id || JSON.parse(localStorage.getItem('user') || '{}').id;

/**
 * Tính thời gian đã trôi qua kể từ khi thông báo được tạo.
 * @param dateString - Ngày tạo thông báo.
 * @returns Chuỗi biểu thị thời gian đã trôi qua.
 */
function getTimeAgo(dateString: string): string {
  const createdDate = new Date(dateString);
  const now = new Date();
  const diffInSeconds = Math.floor((now.getTime() - createdDate.getTime()) / 1000);

  if (diffInSeconds < 60) {
    return `Vừa xong`;
  } else if (diffInSeconds < 3600) {
    const minutes = Math.floor(diffInSeconds / 60);
    return `${minutes} phút trước`;
  } else if (diffInSeconds < 86400) {
    const hours = Math.floor(diffInSeconds / 3600);
    return `${hours} giờ trước`;
  } else {
    const days = Math.floor(diffInSeconds / 86400);
    return `${days} ngày trước`;
  }
}

/**
 * Đánh dấu thông báo là đã đọc khi được click.
 * @param notification - Đối tượng thông báo được click.
 */
async function markAsRead(notification: Notification) {
  if (!notification.isRead) {
    try {
      console.log(`Marking notification ID ${notification.id} as read.`);
      await notificationApi.readNotification(notification.id);

      // Cập nhật trực tiếp thuộc tính isRead
      notification.isRead = true;

      console.log(`Notification ID ${notification.id} marked as read.`);
    } catch (error) {
      console.error('Failed to mark notification as read:', error);
    }
  }
}

/**
 * Lấy thông báo từ API và xử lý chúng.
 */
async function fetchNotifications() {
  if (userId) {
    try {
      console.log(`Fetching notifications for user ID ${userId}.`);
      const response = await notificationApi.getAllByUserId(userId);
      
      // Giả sử cấu trúc phản hồi API là { data: { result: { data: Notification[] } } }
      const fetchedNotifications: Notification[] = response.data.result.data;

      // Xử lý mỗi thông báo để bao gồm thuộc tính timeAgo và làm cho nó phản ứng
      notifications.value = fetchedNotifications
        .map((notification: Notification) => reactive({
          ...notification,
          isRead: Boolean(notification.isRead),
          timeAgo: getTimeAgo(notification.createdDate),
        }))
        .sort((a: Notification, b: Notification) => new Date(b.createdDate).getTime() - new Date(a.createdDate).getTime());

      console.log('Notifications fetched and processed:', notifications.value);
    } catch (error) {
      console.error('Failed to fetch notifications:', error);
    }
  } else {
    console.warn('User ID not found. Cannot fetch notifications.');
  }
}

// Lấy thông báo khi component được mounted
onMounted(fetchNotifications);
</script>

<style scoped>
.notification-container {
  padding: 20px;
}

.notification-header {
  font-size: 24px;
  font-weight: 600;
  color: #333333;
  font-family: "Segoe UI", Tahoma, Geneva, Verdana, sans-serif;
  margin-bottom: 20px;
  text-align: center;
}

.notifications-list {
  max-width: 600px;
  margin: 0 auto;
}

.notification {
  display: flex;
  align-items: center;
  padding: 12px 16px;
  border-bottom: 1px solid #e1e9ee;
  background-color: #ffffff;
  transition: background-color 0.3s, opacity 0.3s;
  cursor: pointer;
}

.notification:hover {
  background-color: #f3f6f8;
}

.notification-read {
  opacity: 0.6; /* Hiển thị mờ cho thông báo đã đọc */
}

.notification-unread {
  opacity: 1; /* Hiển thị rõ cho thông báo chưa đọc */
}

.notification-icon {
  width: 48px;
  height: 48px;
  margin-right: 12px;
}

.notification-text {
  display: flex;
  flex-direction: column;
  color: #3b4045;
}

.notification-text p {
  margin: 0;
  font-size: 15px;
  line-height: 1.4;
}

.unread-text {
  font-weight: bold; /* Chữ đậm cho thông báo chưa đọc */
}

.time {
  font-size: 12px;
  color: #868a8f;
  margin-top: 4px;
}

.no-notifications {
  text-align: center;
  font-size: 14px;
  color: #868a8f;
  margin-top: 20px;
}
</style>
