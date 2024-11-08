export interface Message {
    id?: number; 
    sender: string; 
    receiver: string; 
    content: string; 
    createdDate?: Date;
}

export interface SendMessageModel {
    sender: string; 
    receiver: string; 
    content: string; 
}
