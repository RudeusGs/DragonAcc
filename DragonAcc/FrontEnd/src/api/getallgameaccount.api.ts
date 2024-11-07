import baseApi from './base.api';

const getallgameaccountAPI = {
    getAllLOL: async () => {
          return await baseApi.get('Lol_Game/get-all');
    },
    getAllNgocRong: async () => {
        return await baseApi.get('NgocRong_Game/get-all');
    },
    getAllPubg: async () => {
        return await baseApi.get('Pubg_Game/get-all');
    },
    getAllTocChien: async () => {
        return await baseApi.get('TocChien_Game/get-all');
    },
    getAllValorant: async () => {
        return await baseApi.get('Valorant_Game/get-all');
    },      
}    

export default getallgameaccountAPI;
