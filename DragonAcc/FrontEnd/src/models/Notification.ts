// src/models/Notification.ts
export interface Notification {
    id: number;
    userIdSend: number;
    userId: number;
    content: string;
    isRead: boolean;
    createdDate: string;
    timeAgo?: string;
  }
  