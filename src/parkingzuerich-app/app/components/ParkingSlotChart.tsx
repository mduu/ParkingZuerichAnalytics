'use client'

import { LinearProgress, Table, Typography } from "@mui/joy";
import { useEffect, useState } from "react";
import { ParkingSlots } from "@/src/models";

const apiUrl: string = 'https://parkingzuerichanalytics.azurewebsites.net//api/parking/';

interface ParkingSlotChartProps {
    selectedParking: string | null,
}

export function ParkingSlotChart({selectedParking}: ParkingSlotChartProps) {
    const [data, setData] = useState<ParkingSlots[] | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<Error | null>(null);

    useEffect(() => {
        fetch(`${apiUrl}${selectedParking}`)
            .then(response => {
                if (!response.ok) {
                    throw new Error(
                        `This is an HTTP error: The status is ${response.status}`
                    );
                }
                return response.json();
            })
            .then(data => {
                data = data?.map((d: ParkingSlots): ParkingSlots  => {
                    return {
                        ...d,
                        timestamp: new Date(d.timestamp)
                    }
                });
                setData(data);
                setLoading(false);
            })
            .catch(error => {
                setError(error);
                setLoading(false);
            });
    }, [selectedParking]);

    if (loading) return <LinearProgress size="sm" variant="soft" />;
    if (error) return <p>Error: {error.message}</p>;
    
    return (
        <div>
            <Typography level="h4">Available parking slots at {selectedParking}:</Typography>
            
            <Table stickyHeader sx={{'& tr > *:not(:first-child)': {textAlign: 'right'}}}>
                <thead>
                <tr>
                    <th>Time</th>
                    <th>Free slots</th>
                    <th>Status</th>
                </tr>
                </thead>
                <tbody>
                {data?.map((d, i) => (
                    <tr key={i}>
                        <td>{d.timestamp.toLocaleString()}</td>
                        <td>{d.countFreeSlots}</td>
                        <td>{d.status}</td>
                    </tr>
                ))}
                </tbody>
            </Table>
        </div>
    )
}