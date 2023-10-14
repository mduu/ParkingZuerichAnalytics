import { Fragment } from "react";
import { ParkingAddress } from "@/src/models";

export function ParkingPicker(props: { parkings: ParkingAddress[] }) {
    return (
        <Fragment>
            <ul>
                {props.parkings.map(p => (
                    <li key={p.parkingName}>{p.title}</li>
                ))}
            </ul>
        </Fragment>
    );
}