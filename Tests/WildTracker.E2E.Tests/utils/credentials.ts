export type Role = 'Viewer' | 'Ranger' | 'Admin';
export type Credentials = {email: string; password: string};

export const CREDENTIALS: Record<Role, Credentials> ={
    Viewer: {email:"viewer@wildtracker.pl", password: "Viewer123!"},
    Ranger: {email: "ranger@wildtracker.pl", password: "Ranger123!"},
    Admin: {email: "admin@wildtracker.pl", password: "Admin123!"}
}