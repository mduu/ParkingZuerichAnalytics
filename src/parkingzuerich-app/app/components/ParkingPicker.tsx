import React, { Fragment } from "react";
import { ParkingAddress } from "@/src/models";
import { Select, Option } from "@mui/joy";

export function ParkingPicker(props: { parkings: ParkingAddress[] }) {
    const handleChange = (
        event: React.SyntheticEvent | null,
        newValue: string | null,
    ) => {
        alert(`You chose "${newValue}"`);
    };
    
    return (
        <Fragment>
            <Select
                onChange={handleChange}
                placeholder="Select a parking"
                variant="soft">
                
                {props.parkings.map(p => (
                    <Option key={p.parkingName} value={p.parkingName}>{p.title}</Option>
                ))}
            </Select>
        </Fragment>
    );
}