import { useNavigate } from 'react-router'
import { useAuth } from '../../auth/authContext'
import { Button, Group, Anchor, Text, Title } from '@mantine/core'

export default function TopHeader() {
  const navigate = useNavigate()
  const { username, logout } = useAuth()

  const handleLogout = () => {
    logout()
    navigate('/login')
  }

  const topHeaderHeight = 48
  const borderRadius = 16

  return (
    <Group h={topHeaderHeight} bg="green" justify="center">
      <Group
        bg="primary"
        w="74%"
        h="100%"
        justify="space-between"
        styles={{
          root: { borderBottomRightRadius: borderRadius },
        }}
      >
        <Title order={1}>Portfolio Dashboard</Title>
        <Group>
          <Anchor href="/">Home</Anchor>
          <Anchor href="/strategy">Strategy</Anchor>
        </Group>
        <Group>
          <Text>Welcome, {username}!</Text>
          <Button onClick={handleLogout}>Logout</Button>
        </Group>
      </Group>
    </Group>
  )
}
