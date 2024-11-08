import axios from 'axios';

const purchasedaccountApi = {
  getPurchasedAccountsByUser: async (userId: number) => {
    try {
      return await axios.get(`https://localhost:7071/api/PurchasedAccount/get-by-user`, {
        params: { userId }
      });
    } catch (error) {
      console.error('Error fetching purchased accounts:', error);
      throw error;
    }
  },
};

export default purchasedaccountApi;
