import apiClient from '@/api/base.api';
import type { AxiosResponse } from 'axios';
import type { AddDepositModel } from '@/models/depositModels';

export default {
    getAllByUserId: async (userId: number) => {
        return await apiClient.get(`Deposit/get-all-by-UserId?id=${userId}`);
  },

  getAll: async () => {
    return await apiClient.get('Deposit/get-all');
},

  updateStatus: async (Id: number) => {
    return await apiClient.post(`Deposit/update-status?Id=${Id}`, {});
},

add: async (model: AddDepositModel): Promise<AxiosResponse> => {
    const formData = new FormData();
    formData.append('UserId', model.UserId?.toString() || '');
    formData.append('TelecomO', model.TelecomO || '');
    formData.append('DepositAmount', model.DepositAmount?.toString() || '');
    formData.append('NumberCard', model.NumberCard || '');
    formData.append('NumberSeri', model.NumberSeri || '');
    formData.append('Status', model.Status || '');

    return await apiClient.postForm('Deposit/add', formData);
  }
};
