export interface ParkingSlots {
    parkingName: string;
    timestamp: Date | string;
    countFreeSlots: number;
    status: string | undefined;
}