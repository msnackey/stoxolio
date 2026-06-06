import { Container, Group, AppShell as Shell, Stack } from '@mantine/core'
import { Outlet } from 'react-router'
import { useRouteHandles } from '../../../lib/react-router/useRouteHandles'
import TopHeader from './TopHeader'

const headerFullHeight = 128

export default function AppShell() {
  const matches = useRouteHandles()
  const active = matches[matches.length - 1]

  const subHeader = active?.subHeader ?? null

  return (
    <Shell header={{ height: headerFullHeight }}>
      <Shell.Header withBorder={false}>
        <Stack gap={0}>
          <TopHeader />
          {subHeader && <Group>{subHeader}</Group>}
        </Stack>
      </Shell.Header>

      <Shell.Main bg="white.1">
        <Container>
          <Outlet />
        </Container>
      </Shell.Main>
    </Shell>
  )
}
