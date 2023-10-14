import React, { Fragment } from "react";
import { ParkingAddress } from "@/src/models";
import { Select, Option } from "@mui/joy";

export function ParkingPicker(props: { parkings: ParkingAddress[] }) {
    return (
        <Fragment>
            <Select defaultValue="dog">
                {props.parkings.map(p => (
                    <Option key={p.parkingName} value={p.parkingName}>{p.title}</Option>
                ))}
            </Select>
        </Fragment>
    );
}