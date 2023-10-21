'use client'

import { useState } from 'react';
import '@fontsource/inter';
import { ParkingPicker, ParkingSlotChart } from '@/app/components';

export default function Home() {
    const [selectedParking, setSelectedParking] = useState<string | null>(null);

    return (
        <div>
            <ParkingPicker
                selectedParking={selectedParking}
                onParkingSelected={(parking) => setSelectedParking(parking)}/>
            <ParkingSlotChart
                selectedParking={selectedParking}/>
        </div>
    )
}
