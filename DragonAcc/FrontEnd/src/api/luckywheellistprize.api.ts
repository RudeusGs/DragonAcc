// src/api/luckywheellistprize.api.ts
import baseApi from './base.api';

export const luckyWheelListPrizeApi = {
  getAllLuckyWheel: async () => {
      const response = await baseApi.get('LuckyWheelListPrize/get-all');
      return response.data.result.data;
  },

  getByIdLuckyWheel: async (id : number) => {
      const response = await baseApi.get(`LuckyWheelListPrize/get-by-user?userId=${id}`);
      console.log("API Response:", response);
      return response.data;
  },
};
