export interface Photo {
  id: number;
  url: string;
  isMain: boolean;
}

export interface Member {
  username: string;
  knownAs: string;
  age: number;
  gender: string;
  introduction: string;
  lookingFor: string;
  interests: string;
  city: string;
  country: string;
  photoUrl: string;
  photos: Photo[];
  lastActive: Date;
}
