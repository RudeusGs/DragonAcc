import apiClient from '@/api/base.api';
import type { AxiosResponse } from 'axios';

const notificationApi = {
  getAllByUserId: async (userId: number): Promise<AxiosResponse> => {
    return await apiClient.get('notification/get-by-user', { userId });
  },

  readNotification: async (id: number): Promise<AxiosResponse> => {
    return await apiClient.put('notification/read', { id });
  },
};

export default notificationApi;
