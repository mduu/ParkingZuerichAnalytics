import { Typography } from "@mui/joy";
import { Fragment } from "react";

interface ParkingSlotChartProps {
    selectedParking: string | null,
}

export function ParkingSlotChart({selectedParking}: ParkingSlotChartProps) {
    
    return (
        <Fragment>
            <Typography level="h4">Chart here!</Typography>
            {selectedParking}
        </Fragment>
    )
}