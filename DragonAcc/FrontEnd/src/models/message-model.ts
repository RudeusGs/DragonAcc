export interface Message {
    id?: number; 
    sender: string; 
    receiver: string; 
    content: string; 
    createdDate?: Date; // Optional for when fetching messages
}

export interface SendMessageModel {
    sender: string; 
    receiver: string; 
    content: string; 
}
