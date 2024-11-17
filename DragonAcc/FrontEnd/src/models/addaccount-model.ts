
export interface GameInforBase {
    id?: number | null;
    userId?: number | null;
    adminUpdate?: number | null;
    gameName?: string | null;
    accountName?: string | null;
    password?: string | null;
    passwordChanged?: string | null;
    description?: string | null;
    price?: number | null;
    image?: string | null;
    status?: string | null;
    noYetMoney?: boolean | null;
    files?: File[];
    comments?: Comment[] | null;
    reactions?: null ;
    createdDate?: Date | null;
    updatedDate?: Date | null;
  }
export interface Lol_Game extends GameInforBase {
    championCount?: number | null;
    skinCount?: number | null;
    rank?: string | null;
}
export interface NgocRong_Game extends GameInforBase {
    server?: number | null;
    planet?: string | null;
}
export interface Pubg_Game extends GameInforBase {
    gunSkinCount?: number | null;
    humanSkinCount?: number | null;
    rank?: string | null;
}
export interface TocChien_Game extends GameInforBase {
    championCount?: number | null;
    skinCount?: number | null;
    rank?: string | null;
} 
export interface Valorant_Game extends GameInforBase {
    gunSkinCount?: number | null;
    championCount?: number | null;
    rank?: string | null;
}