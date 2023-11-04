import React, { Fragment, useState, useEffect } from "react";
import { ParkingAddress } from "@/src/models";
import { FormControl, InputLabel, LinearProgress, MenuItem, Select, SelectChangeEvent } from "@mui/material";

const apiUrl: string = 'https://parkingzuerichanalytics.azurewebsites.net/api/parking';

interface ParkingPickerProps {
    selectedParking: string | null,
    onParkingSelected: (selectedParking: string | null) => void,
}

export function ParkingPicker({selectedParking, onParkingSelected}: ParkingPickerProps) {
    const [data, setData] = useState<ParkingAddress[] | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<Error | null>(null);

    useEffect(() => {
        fetch(apiUrl)
            .then(response => {
                if (!response.ok) {
                    throw new Error(
                        `This is an HTTP error: The status is ${response.status}`
                    );
                }
                return response.json();
            })
            .then(data => {
                setData(data);
                setLoading(false);
            })
            .catch(error => {
                setError(error);
                setLoading(false);
            });
    }, []);

    const handleChange = (event: SelectChangeEvent) => {
        onParkingSelected(event.target.value);
    };

    if (loading) return <LinearProgress/>;
    if (error) return <p>Error: {error.message}</p>;

    return (
        <div style={{ padding: 10, paddingTop: 15 }}>
            <FormControl variant="standard" fullWidth={true}>
                <InputLabel id="demo-simple-select-standard-label">Parking</InputLabel>
                <Select
                    onChange={handleChange}
                    value={selectedParking ?? ''}
                    variant="filled">

                    {data && data.map(p => (
                        <MenuItem key={p.parkingName} value={p.parkingName}>{p.title}</MenuItem>
                    ))}
                </Select>
            </FormControl>
        </div>
    );
}