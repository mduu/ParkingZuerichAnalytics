'use client'

import { useState } from 'react';
import Button from "@mui/material/Button";

export default function Home() {
    const [name, setName] = useState('Marc');
    return (
        <div>
            <h1>Hi from Testpage</h1>
            <p>
                Name: {name}
            </p>
            <div>
                <Button onClick={() => setName("Tania")}>Set name to &quot;Tania&quot;</Button>
            </div>
        </div>
    );
}