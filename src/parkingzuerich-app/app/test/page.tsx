'use client'

import { useState } from "react";
import { Button } from "@mui/joy";

export default function Home() {
    const [name, setName] = useState("Marc");
    return (
        <div>
            <h1>Hi from Testpage</h1>
            <p>
                Name: {name}
            </p>
            <div>
                <Button onClick={() => setName("Tania")}>Set name to "Tania"</Button>
            </div>
        </div>
    );
}