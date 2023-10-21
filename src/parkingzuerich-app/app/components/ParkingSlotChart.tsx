'use client'

import { useEffect, useState } from "react";
import { ParkingSlots } from "@/src/models";
import {
    LinearProgress,
    Paper,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TableRow
} from "@mui/material";

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
                data = data?.map((d: ParkingSlots): ParkingSlots => {
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

    if (loading) return <LinearProgress/>;
    if (error) return <p>Error: {error.message}</p>;

    return (
        <TableContainer component={Paper}>
            <Table stickyHeader sx={{'& tr > *:not(:first-child)': {textAlign: 'right'}}}>
                <TableHead>
                    <TableRow>
                        <TableCell>Time</TableCell>
                        <TableCell>Free slots</TableCell>
                        <TableCell>Status</TableCell>
                    </TableRow>
                </TableHead>
                <TableBody>
                    {data?.map((d, i) => (
                        <TableRow
                            key={i}
                            sx={{'&:last-child td, &:last-child th': {border: 0}}}>
                            <TableCell>{d.timestamp.toLocaleString()}</TableCell>
                            <TableCell align="right">{d.countFreeSlots}</TableCell>
                            <TableCell align="right">{d.status}</TableCell>
                        </TableRow>
                    ))}
                </TableBody>
            </Table>
        </TableContainer>
    )
}