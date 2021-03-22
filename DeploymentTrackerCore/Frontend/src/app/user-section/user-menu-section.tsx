import LogoutButton from './logout-button';
import MentionsButton from './mentions-button';

const UserMenuSection = (): JSX.Element => (
    <div style={{ display: 'flex' }}>
        <div>
            <MentionsButton />
        </div>
        <LogoutButton />
    </div>
);

export default UserMenuSection;
