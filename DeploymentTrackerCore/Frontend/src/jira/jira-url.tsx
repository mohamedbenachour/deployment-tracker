import { Button } from 'antd';

interface JiraUrlProps {
    url: string;
    style: Record<string, any>;
}

const JiraUrl = ({ url, style }: JiraUrlProps): JSX.Element => (
    <>
        {url && (
        <a href={url} target="_blank" style={style} rel="noreferrer">
            <Button size="small" type="link">
                Jira
            </Button>
        </a>
        )}
    </>
);

export default JiraUrl;
