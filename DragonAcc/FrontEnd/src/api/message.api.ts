// message.api.ts
import baseApi from './base.api';
import type { Message, SendMessageModel } from '../models/message-model'; // Adjust the path to your types
import type { AxiosResponse } from 'axios';

const messageApi = {
    sendMessage: async (message: SendMessageModel): Promise<AxiosResponse> => {
        return await baseApi.post('message/send', message);
    },
    getConversation: async (user1: string, user2: string): Promise<AxiosResponse> => {
        return await baseApi.get('message/get-conversation', {
            params: { user1, user2 },
        });
    },
    getMessageById: async (id: number): Promise<AxiosResponse> => {
        return await baseApi.get('message/get-by-id', {
            params: { id },
        });
    },
    deleteMessage: async (id: number): Promise<AxiosResponse> => {
        return await baseApi.delete(`message/delete?id=${id}`);
    },
};

export default messageApi;
