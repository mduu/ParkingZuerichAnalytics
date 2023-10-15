'use client'

import { useState } from "react";
import '@fontsource/inter';
import { Typography } from "@mui/joy";
import { ParkingPicker, ParkingSlotChart } from "@/app/components";

export default function Home() {
    const [selectedParking, setSelectedParking] = useState<string | null>(null);
    
    return (
        <div>
            <Typography level="h1">Parking Zürich Analytics</Typography>
            <ParkingPicker 
                selectedParking={selectedParking} 
                onParkingSelected={(parking) => setSelectedParking(parking)}/>
            <ParkingSlotChart
                selectedParking={selectedParking} />
        </div>
    )
}
