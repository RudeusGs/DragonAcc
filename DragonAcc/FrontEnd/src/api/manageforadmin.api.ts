import baseApi from './base.api';

const ManageUpdateForAdminAPI = {
  ManageUpdateLOL: async (id: number) => {
    try {
      const response = await baseApi.post(`Lol_Game/update-for-admin?id=${id}`, {} );
      return response.data;
    } catch (error) {
      console.error('Error updating League of Legends account:', error);
      throw error;
    }
  },

  ManageUpdateNgocRong: async (id: number) => {
    try {
      const response = await baseApi.post(`NgocRong_Game/update-for-admin?id=${id}`, {} );
      return response.data;
    } catch (error) {
      console.error('Error updating Ngọc Rồng account:', error);
      throw error;
    }
  },

  ManageUpdatePubg: async (id: number) => {
    try {
      const response = await baseApi.post(`Pubg_Game/update-for-admin?id=${id}`, {} );
      return response.data;
    } catch (error) {
      console.error('Error updating PUBG account:', error);
      throw error;
    }
  },

  ManageUpdateTocChien: async (id: number) => {
    try {
      const response = await baseApi.post(`TocChien_Game/update-for-admin?id=${id}`, {} );
      return response.data;
    } catch (error) {
      console.error('Error updating Tốc Chiến account:', error);
      throw error;
    }
  },

  ManageUpdateValorant: async (id: number) => {
    try {
      const response = await baseApi.post(`Valorant_Game/update-for-admin?id=${id}`, {} );
      return response.data;
    } catch (error) {
      console.error('Error updating Valorant account:', error);
      throw error;
    }
  },
};

export default ManageUpdateForAdminAPI;
