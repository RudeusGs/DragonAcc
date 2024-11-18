// src/signalr.ts
import { HubConnectionBuilder, LogLevel, HubConnection } from '@microsoft/signalr';

let connection: HubConnection;

export function startSignalRConnection(
  auctionId: number,
  onReceiveBid: Function,
  onAuctionEnded: Function
) {
  connection = new HubConnectionBuilder()
    .withUrl('https://localhost:7071/auctionHub') // Đảm bảo URL này đúng với backend của bạn
    .configureLogging(LogLevel.Information)
    .build();

  connection.on('ReceiveBid', (bid) => {
    onReceiveBid(bid);
  });

  connection.on('AuctionEnded', (endedAuctionId) => {
    if (endedAuctionId === auctionId) {
      onAuctionEnded();
    }
  });

  connection.start()
    .then(() => {
      console.log('SignalR connected.');
      // Tham gia vào nhóm cụ thể cho phiên đấu giá
      connection.invoke('JoinGroup', `Auction_${auctionId}`);
    })
    .catch(err => console.error('SignalR connection error: ', err));
}

export function stopSignalRConnection(auctionId: number) {
  if (connection) {
    connection.invoke('LeaveGroup', `Auction_${auctionId}`);
    connection.stop();
  }
}
