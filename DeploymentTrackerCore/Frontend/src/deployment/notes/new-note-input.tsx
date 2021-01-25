import { Mentions } from 'antd';
import React from 'react';
import ApiUser from '../../shared/definitions/api-user';
import { getJSON } from '../../utils/io';

interface NewNoteInputProps {
    onSave: (input: string) => void;
}

const loadUsers = (
    setIsLoading: (loading: boolean) => void,
    setUsers: (users: ApiUser[]) => void,
) => {
    setIsLoading(true);

    getJSON<ApiUser[]>(
        '/api/user',
        (users) => {
            setUsers(users ?? []);
            setIsLoading(false);
        },
        () => {},
    );
};

const isSaveButtonPress = ({
    key,
    ctrlKey,
}: React.KeyboardEvent<HTMLTextAreaElement>) => ctrlKey && key === 'Enter';

const handleKeyPress = (
    event: React.KeyboardEvent<HTMLTextAreaElement>,
    onSave: (input: string) => void,
): void => {
    if (isSaveButtonPress(event)) {
        onSave(event.currentTarget.value);
    }
};

const NewNoteInput = ({ onSave }: NewNoteInputProps): JSX.Element => {
    const [isLoading, setIsLoading] = React.useState(false);
    const [users, setUsers] = React.useState<ApiUser[]>([]);
    const helpText = 'Ctrl+Enter to Save.';

    const onSearch = (input: string, _: string) => {
        if (isLoading || users.length > 0) {
            return;
        }

        loadUsers(setIsLoading, setUsers);
    };

    return (
        <Mentions
          placeholder={helpText}
          title={helpText}
          autoSize={{ minRows: 3 }}
          loading={isLoading}
          onSearch={onSearch}
          onKeyPress={(event: React.KeyboardEvent<HTMLTextAreaElement>) => handleKeyPress(event, onSave)}
          prefix="<@"
        >
            {users.map((user) => (
                <Mentions.Option key={user.username} value={`${user.username}>`}>
                    {user.name}
                </Mentions.Option>
      ))}
        </Mentions>
    );
};

export default NewNoteInput;
