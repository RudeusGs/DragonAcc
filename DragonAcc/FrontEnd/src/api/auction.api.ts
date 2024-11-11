import baseApi from './base.api';
import type { AddAuctionModel, UpdateAuctionModel, UpdateCurrentPriceModel } from '@/models/auction-model';

const AuctionApi = {
  getAllAuction: async () => {
    return await baseApi.get('Auction/get-all');
  },
  getByIdAuction: async (id: number) => {
    return await baseApi.get(`Auction/get-by-id?id=${id}`);
  },
  addAuction: async (model: AddAuctionModel) => {
    return await baseApi.post('Auction/add', model);
  },
  updateAuction: async (model: UpdateAuctionModel) => {
    return await baseApi.put('Auction/update', model);
  },
  updateCurrentPriceAuction: async (model: UpdateCurrentPriceModel) => {
    return await baseApi.post('Auction/update-current-price', model);
    
  },
  getWinnerByAuctionId: async (auctionId: number): Promise<any> => {
    const response = await baseApi.get(`Auction/get-winner-by-auction-id`, { auctionId });
    return response.data;
  },
};

export default AuctionApi;

