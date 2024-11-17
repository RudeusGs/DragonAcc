export interface AddAuctionModel {
  auctionName?: string;
  prize?: string;
  files?: File[];
  image?: string;
  startPrice?: string;
  currentPrice?: string;
  startDateTime?: string;
  timeAuction?: string;
  status?: string;
  winner?: number;
}

export interface UpdateAuctionModel {
  id?: number;
  prize?: string;
  auctionName?: string;
  files?: File[];
  image?: string;
  startPrice?: string;
  startDateTime?: string;
  timeAuction?: string;
}
// Trong '@/models/auctionmodel.ts'
// Trong '@/models/auctionmodel.ts'
export interface AuctionModel {
  id: number;
  auctionName: string;
  prize: string;
  image: string;
  startPrice: number;
  currentPrice: number;
  startDateTime: string;
  timeAuction: string;
  status: boolean;
  winnerId?: number;
}

