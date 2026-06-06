import { Button, Group, Modal, Text } from '@mantine/core'

interface Props {
  opened: boolean
  title: string
  message: string
  confirmLabel?: string
  confirmColor?: string
  onConfirm: () => void
  onClose: () => void
}

export default function ConfirmModal({
  opened,
  title,
  message,
  confirmLabel = 'Confirm',
  confirmColor = 'red',
  onConfirm,
  onClose,
}: Props) {
  return (
    <Modal opened={opened} onClose={onClose} title={'Delete ' + title} centered>
      <Text size="sm">
        Delete{' '}
        <Text fw={700} span>
          {message}
        </Text>
        ? This cannot be undone.
      </Text>
      <Group justify="flex-end" mt="md">
        <Button variant="default" onClick={onClose}>
          Cancel
        </Button>
        <Button color={confirmColor} onClick={onConfirm}>
          {confirmLabel}
        </Button>
      </Group>
    </Modal>
  )
}
