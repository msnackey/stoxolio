import { createTheme, MantineProvider } from '@mantine/core'
import type { PropsWithChildren } from 'react'
import "./customStyles.css"

const theme = createTheme({
  fontFamily: 'Open Sans, sans-serif',
  primaryColor: 'green',
})

export default function ThemeProvider({ children }: PropsWithChildren) {
  return <MantineProvider theme={theme}>{children}</MantineProvider>
}
