import { Fragment, useState, useEffect } from "react";
import { ParkingAddress } from "@/src/models";
import { Select, Option } from "@mui/joy";

const apiUrl: string = 'https://parkingzuerichanalytics.azurewebsites.net/api/parking';

interface ParkingPickerProps {
    selectedParking: string | null,
    onParkingSelected: (selectedParking: string | null) => void,
}

export function ParkingPicker({selectedParking, onParkingSelected} : ParkingPickerProps) {
    const [data, setData] = useState<ParkingAddress[] | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<Error | null>(null);

    useEffect(() => {
        fetch(apiUrl)
            .then(response => response.json())
            .then(data => {
                setData(data);
                setLoading(false);
            })
            .catch(error => {
                setError(error);
                setLoading(false);
            });
    }, []);

    const handleChange = (event: React.SyntheticEvent | null, newValue: string | null) => {
        onParkingSelected(newValue);
    };

    if (loading) return <p>Loading...</p>;
    if (error) return <p>Error: {error.message}</p>;
    
    return (
        <Fragment>
            <Select
                onChange={handleChange}
                value={selectedParking}
                placeholder="Select a parking"
                variant="soft">
                
                {data && data.map(p => (
                    <Option key={p.parkingName} value={p.parkingName}>{p.title}</Option>
                ))}
            </Select>
        </Fragment>
    );
}