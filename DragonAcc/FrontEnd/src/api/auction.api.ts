import baseApi from './base.api';
import type { AddAuctionModel, UpdateAuctionModel, UpdateCurrentPriceModel } from '@/models/auctionmodel';

const AuctionApi = {
  getAllAuction: async () => {
    return await baseApi.get('Auction/get-all');
  },

  addAuction: async (model: AddAuctionModel) => {
    return await baseApi.post('Auction/add', model);
  },
  getByIdAuction: async (id: number) => {
    return await baseApi.get(`Auction/get-by-id?id=${id}`);
  },
  updateAuction: async (model: UpdateAuctionModel) => {
    return await baseApi.put('Auction/update', model);
  },

  updateCurrentPriceAuction: async (model: UpdateCurrentPriceModel) => {
    return await baseApi.post('Auction/update-current-price', model);
  },

  getWinnerByAuctionId: async (auctionId: number): Promise<any> => {
    const response = await baseApi.get(`Auction/get-winner-by-auction-id`, { params: { auctionId } });
    return response.data;
  },

  getAllAuctionDetail: async (auctionId: number) => {
    return await baseApi.get(`Auction/get-all-auction-detail?auctionId=${auctionId}`);
  },

  addAuctionDetail: async (model: any) => {
    return await baseApi.post('Auction/add-auction-detail', model);
  },

  endAuction: async (id: number) => {
    return await baseApi.put(`Auction/end-auction?id=${id}`, {}); // Thêm một đối tượng rỗng {} làm body
  },
  getUserByID: async (id: number) => {
    return await baseApi.get(`AccountWebsite/get-by-id?id=${id}`);
  }
};

export default AuctionApi;
