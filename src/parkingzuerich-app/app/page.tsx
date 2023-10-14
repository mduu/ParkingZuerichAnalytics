'use client'

import '@fontsource/inter';
import { ParkingAddress } from "@/src/models";
import { ParkingPicker } from "@/app/components";

async function getData(): Promise<ParkingAddress[]> {
    const res = await fetch('https://parkingzuerichanalytics.azurewebsites.net/api/parking')
    // The return value is *not* serialized
    // You can return Date, Map, Set, etc.

    if (!res.ok) {
        // This will activate the closest `error.js` Error Boundary
        throw new Error('Failed to fetch data');
    }

    return res.json();
}

export default async function Home() {
    const data = await getData();

    return (
        <main>
            <ParkingPicker parkings={data} />
        </main>
    )
}
