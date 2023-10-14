import { ParkingAddress } from "@/src/models";

export function ParkingPicker(props: { parkings: ParkingAddress[] }) {
    return (
        <div>
            <h3>I am the ParkingPicker</h3>
            <ul>
                {props.parkings.map(p => (
                    <li key={p.parkingName}>{p.title}</li>
                ))}
            </ul>
        </div>
    );
}