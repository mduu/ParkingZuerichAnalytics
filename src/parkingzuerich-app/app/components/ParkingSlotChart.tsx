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
import { LineChart } from "@mui/x-charts";
import Typography from "@mui/material/Typography";
import { Colorize, ColorLens } from "@mui/icons-material";

const apiUrl: string = 'https://parkingzuerichanalytics.azurewebsites.net//api/parking/';

interface ParkingSlotChartProps {
    selectedParking: string | null,
}

function subtractDays(date:Date, days: number) {
    date.setDate(date.getDate() - days);
    return date;
}

export function ParkingSlotChart({selectedParking}: ParkingSlotChartProps) {
    const [data, setData] = useState<ParkingSlots[] | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<Error | null>(null);

    useEffect(() => {
        const queryFrom= subtractDays(new Date(), 14).toISOString();
        fetch(`${apiUrl}${selectedParking}?from=${queryFrom}`)
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
    if (!data || data.length == 0) return;
    
    const currentWeekStart = subtractDays(new Date(), 7);
    
    const yCurrentWeek = data
        .filter(p => p.timestamp > currentWeekStart)
        .map(p => (p.countFreeSlots));
    const yWeekBefore = data
        .filter(p => p.timestamp < currentWeekStart)
        .map(p => (p.countFreeSlots));

    const xLabels = data
        .filter(p => p.timestamp > currentWeekStart)
        .map(p => (p.timestamp));
    
    return (
        <div>
            {data &&
                <LineChart
                    xAxis={[{ 
                        scaleType: 'time',
                        data: xLabels,
                        disableTicks: true,
                        disableLine: true,
                    }]}
                    series={[
                        { 
                            data: yCurrentWeek, 
                            label: 'This week free parking slots', 
                            showMark: false,
                            color: 'DodgerBlue',
                            curve: "catmullRom",
                        },
                        { 
                            data: yWeekBefore, 
                            label: 'Week before free parking slots', 
                            showMark: false,
                            color: 'DeepSkyBlue',
                            curve: "catmullRom",
                        },
                    ]}
                    height={300}
                />
            }

            {data &&
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
                            {data.map((d, i) => (
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
            }
        </div>
    )
}