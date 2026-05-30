import { Group, AppShell as Shell, Stack } from '@mantine/core'
import LoadingOverlayComponent from './LoadingOverlay'

const headerFullHeight = 128
const topHeaderHeight = 32
const borderRadius = 16

export default function AppShellLoader() {
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
        </Stack>
      </Shell.Header>

      <Shell.Main bg="white.1">
        <LoadingOverlayComponent />
      </Shell.Main>
    </Shell>
  )
}
