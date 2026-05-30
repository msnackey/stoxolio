import { Group, AppShell as Shell, Stack, Text, Title } from '@mantine/core'
import { isRouteErrorResponse, useRouteError, useSearchParams } from 'react-router'
import { ValidationError } from '../../../types/validationError'

const borderRadius = 16
const topHeaderHeight = 32

interface Props {
  title?: string
  offSet: number
}

export default function GlobalErrorPage({ title, offSet }: Props) {
  let message
  let status
  let statusText
  const error = useRouteError()
  const [searchParams] = useSearchParams()
  const debug = searchParams.get('debug') === 'true'

  if (isRouteErrorResponse(error)) {
    message = error.data.message
    status = error.status
    statusText = error.statusText
  } else if (error instanceof ValidationError) {
    message = error.message
    status = error.status
    statusText = error.statusText
  } else {
    message = 'Onverwachte error.'
  }

  return (
    <Shell header={{ height: topHeaderHeight }}>
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
        <Stack p="xl" pt={offSet}>
          {title && <Title>{title}</Title>}
          <Stack>
            <Text fz="h4">{message}</Text>

            {debug && (
              <Stack gap="xs">
                <Text fw="bold" mt="md" c="red.0">
                  Debug informatie
                </Text>
                <pre style={{ marginTop: 0 }}>
                  <Text>Error status: {status}</Text>
                  <Text>Error bericht: {JSON.stringify(statusText, null, 2)}</Text>
                </pre>
              </Stack>
            )}
          </Stack>
        </Stack>
      </Shell.Main>
    </Shell>
  )
}
