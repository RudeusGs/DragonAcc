import baseApi from './base.api';

const PurchasedgameaccountAPI = {
    purchasedLOL: async (gameAccountId: number, userId: number) => {
        const requestBody = {
            Id: gameAccountId,
            UserId: userId,
        };
          return await baseApi.post('Lol_Game/Purchase', requestBody);
    },
    purchasedNgocRong: async (gameAccountId: number, userId: number) => {
        const requestBody = {
            Id: gameAccountId,
            UserId: userId,
        };
        return await baseApi.post('NgocRong_Game/Purchase', requestBody);
    },
    purchasedPubg: async (gameAccountId: number, userId: number) => {
        const requestBody = {
            Id: gameAccountId,
            UserId: userId,
        };      
        return await baseApi.post('Pubg_Game/Purchase', requestBody);
    },
    purchasedTocChien: async (gameAccountId: number, userId: number) => {
        const requestBody = {
            Id: gameAccountId,
            UserId: userId,
        };
        
        return await baseApi.post('TocChien_Game/Purchase', requestBody);
    },
    purchasedValorant: async (gameAccountId: number, userId: number) => {
        const requestBody = {
            Id: gameAccountId,
            UserId: userId,
        };
           
        return await baseApi.post('Valorant_Game/Purchase', requestBody);
    },      
}    

export default PurchasedgameaccountAPI;
