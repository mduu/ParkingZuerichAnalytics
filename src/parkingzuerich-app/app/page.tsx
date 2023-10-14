'use client'

import '@fontsource/inter';
import { ParkingAddress } from "@/src/models";
import { ParkingPicker } from "@/app/components";
import { Typography } from "@mui/joy";

async function getData(): Promise<ParkingAddress[]> {
    const res = await fetch('https://parkingzuerichanalytics.azurewebsites.net/api/parking')

    if (!res.ok) {
        // This will activate the closest `error.js` Error Boundary
        throw new Error('Failed to fetch data');
    }

    return res.json();
}

export default async function Home() {
    const data = await getData();

    return (
        <div>
            <Typography level="h1">Parking Zürich Analytics</Typography>
            <Typography level="body-md">Choose a parking:</Typography>
            <ParkingPicker parkings={data}/>
        </div>
)
}
