import baseApi from './base.api';

const getpostbyuserid = {
    getLolAccountsByUser: async (userId: number) => {
          return await baseApi.get(`Lol_Game/get-by-user/${userId}`);
    },
    getNgocRongAccountsByUser: async (userId: number) => {
        return await baseApi.get(`NgocRong_Game/get-by-user/${userId}`);
    },
    getPubgAccountsByUser: async (userId: number) => {
        return await baseApi.get(`Pubg_Game/get-by-user/${userId}`);
    },
    getTocChienAccountsByUser: async (userId: number) => {
        return await baseApi.get(`TocChien_Game/get-by-user/${userId}`);
    }   ,
    getValorantAccountsByUser: async (userId: number) => {
        return await baseApi.get(`Valorant_Game/get-by-user/${userId}`);
    },
};

export default getpostbyuserid;
