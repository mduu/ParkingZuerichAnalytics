import type { Metadata } from 'next'
import { Inter } from 'next/font/google'
import AppBar from '@mui/material/AppBar';
import Box from '@mui/material/Box';
import Toolbar from '@mui/material/Toolbar';
import Typography from '@mui/material/Typography';
import IconButton from '@mui/material/IconButton';
import MenuIcon from '@mui/icons-material/Menu';
import { Fragment } from "react";
import { CssBaseline } from "@mui/material";

const inter = Inter({subsets: ['latin']})

export const metadata: Metadata = {
    title: 'Parking Zurich Analytics',
    description: 'Analyze free parking spaces in the City of Zurich',
}

export default function RootLayout({
                                       children,
                                   }: {
    children: React.ReactNode
}) {
    return (
        <Fragment>
            <html lang="en">
            <body className={inter.className}>
            <CssBaseline/>
            <Box sx={{flexGrow: 1}}>
                <AppBar position="static">
                    <Toolbar>
                        <IconButton
                            size="large"
                            edge="start"
                            color="inherit"
                            aria-label="menu"
                            sx={{mr: 2}}
                        >
                            <MenuIcon/>
                        </IconButton>
                        <Typography variant="h6" component="div" sx={{flexGrow: 1}}>
                            Parking Zurich Analytics
                        </Typography>
                    </Toolbar>
                </AppBar>
                {children}
            </Box>
            </body>
            </html>
        </Fragment>
    )
}
