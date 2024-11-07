import baseApi from './base.api';

const getfullname = {
    getfullnameLol: async (id: number) => {
        return await baseApi.get(`Lol_Game/get-full-name?id=${id}`);
    },    
    getfullnameNgocRong: async (id: number) => {
        return await baseApi.get(`NgocRong_Game/get-full-name?id=${id}`);
    },
    getfullnamePubg: async (id: number) => {
        return await baseApi.get(`Pubg_Game/get-full-name?id=${id}`);
    },
    getfullnameTocChien: async (id: number) => {
        return await baseApi.get(`TocChien_Game/get-full-name?id=${id}`);
    },
    getfullnameValorant: async (id: number) => {
        return await baseApi.get(`Valorant_Game/get-full-name?id=${id}`);
    },      
}    

export default getfullname;
