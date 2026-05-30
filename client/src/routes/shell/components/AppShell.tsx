import { Group, AppShell as Shell, Stack } from '@mantine/core'
import { Outlet } from 'react-router'
import { useRouteHandles } from '../../../lib/react-router/useRouteHandles'

const headerFullHeight = 128
const topHeaderHeight = 32
const borderRadius = 16

export default function AppShell() {
  const matches = useRouteHandles()
  const active = matches[matches.length - 1]

  const subHeader = active?.subHeader ?? null

  return (
    <Shell header={{ height: headerFullHeight }}>
      <Shell.Header withBorder={false}>
        <Stack gap={0}>
          {/* Top header */}
          <Group h={topHeaderHeight} bg="green">
            <Group
              bg="primary"
              w="74%"
              h="100%"
              styles={{
                root: { borderBottomRightRadius: borderRadius },
              }}
            />
          </Group>

          {subHeader && <Group>{subHeader}</Group>}
        </Stack>
      </Shell.Header>

      <Shell.Main bg="white.1">
        <Outlet />
      </Shell.Main>
    </Shell>
  )
}
