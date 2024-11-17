// src/api/statistical.api.ts
import baseapi from './base.api'; 

export const getStatisticsByUserId = {
    getByUserId: async (id: number) => {
        try {
            const response = await baseapi.get(`Statistical/get-by-user?id=${id}`);
            return response.data;
        } catch (error) {
            console.error('Error fetching statistics:', error);
            return null;
        }
    },
}
 
