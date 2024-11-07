//addgameaccount.api.ts
import baseapi from './base.api'; 

export const addLol_Game = async (formData: FormData) => {
    const response = await baseapi.postForm('Lol_Game/add-for-user', formData);
    return response.data;
};
export const addNgocRong_Game = async (formData: FormData) => {
    const response = await baseapi.postForm('NgocRong_Game/add-for-user', formData);
    return response.data;
};
export const addPubg_Game = async (formData: FormData) => {
    const reponse  = await baseapi.postForm('Pubg_Game/add-for-user', formData);
    return reponse.data
};
export const addTocChien_Game = async (formData: FormData) => {
    const reponse  = await baseapi.postForm('TocChien_Game/add-for-user', formData);
    return reponse.data
};
export const addValorant_Game = async (formData: FormData) => {
    const reponse  = await baseapi.postForm('Valorant_Game/add-for-user', formData);
    return reponse.data
}
